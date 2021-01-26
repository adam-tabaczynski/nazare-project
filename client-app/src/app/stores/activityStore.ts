import {
  HubConnection,
  HubConnectionBuilder,
  LogLevel,
} from "@microsoft/signalr";
import {
  action,
  computed,
  observable,
  runInAction,
  makeObservable,
  reaction,
  toJS,
} from "mobx";
import { SyntheticEvent } from "react";
import { toast } from "react-toastify";
import { history } from "../..";
import agent from "../api/agent";
import { createAttendee, setActivityProps } from "../common/util/util";
import { IActivity } from "../models/activity";
import { RootStore } from "./rootStore";

const LIMIT = 2;

export default class ActivityStore {
  rootStore: RootStore;
  constructor(rootStore: RootStore) {
    this.rootStore = rootStore;
    makeObservable(this);

    // setup for filters.
    // I want app to automatically react to filter changes - this piece of code is
    // required to do it.
    // reaction observes the observables predicate.keys() - if theses keys changes,
    // then the expression of zeroing this.page, etc. will run.
    reaction(
      // this function will be observed to check if value changed.
      () => this.predicate.keys(),
      // these are side effects that will kickoff with each value change
      // in function above.
      () => {
        this.page = 0;
        this.activityRegistry.clear();
        this.loadActivities();
      }
    );
  }

  @observable activityRegistry = new Map();
  @observable activity: IActivity | null = null;
  // variable responsible for showing loading indicator while initially loading
  // data.
  @observable loadingInitial = false;
  // variable for loading indicator in buttons.
  @observable submitting = false;
  // variable that points to which delete button has been clicked so the
  // loading indicator can show up only on him.
  @observable target = "";
  // this one will appear in case of attending and unattending an event.
  @observable loading = false;
  @observable.ref hubConnection: HubConnection | null = null;
  @observable activityCount = 0;
  @observable page = 0;
  // this will be a map, because the concept is to keep the filters in one place as pairs,
  // isGoing - true, isHost - false, limit - 7, etc.
  @observable predicate = new Map();

  @computed get totalPages() {
    return Math.ceil(this.activityCount / LIMIT);
  }

  @computed get axiosParams() {
    // interface that defines methods to work with query string of an URL.
    const params = new URLSearchParams();
    // adding key-value pairs from map of predicates.
    params.append("limit", String(LIMIT));
    params.append("offset", `${this.page ? this.page * LIMIT : 0}`);
    this.predicate.forEach((value, key) => {
      // value will be a date, so I translate it to string, string format is required.
      if (key === "startDate") {
        params.append(key, value.toISOString());
      } else {
        params.append(key, value);
      }
    });
    return params;
  }

  @action setPredicate = (predicate: string, value: string | Date) => {
    this.predicate.clear();
    if (predicate !== "all") {
      this.predicate.set(predicate, value);
    }
  };

  @action setPage = (page: number) => {
    this.page = page;
  };

  @action createHubConnection = (activityId: string) => {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(process.env.REACT_APP_API_CHAT_URL!, {
        accessTokenFactory: () => this.rootStore.commonStore.token!,
      })
      .configureLogging(LogLevel.Information)
      .build();

    this.hubConnection
      .start()
      .then(() => console.log(this.hubConnection!.state))
      .then(() => {
        if (this.hubConnection!.state === "Connected") {
          console.log("Attempting to join group");
          this.hubConnection!.invoke("AddToGroup", activityId);
        }
      })
      .catch((error) => console.log("Error establishing connection: ", error));

    this.hubConnection.on("ReceiveComment", (comment) => {
      runInAction(() => {
        this.activity!.comments.push(comment);
      });
    });

    this.hubConnection.on("Send", (message) => {
      // toast.info(message);
    });
  };

  @action stopHubConnection = () => {
    this.hubConnection!.invoke("RemoveFromGroup", this.activity!.id)
      .then(() => {
        runInAction(() => {
          this.activity = null;
        })
        this.hubConnection!.stop();
      })
      .then(() => console.log("Connection stopped"))
      .catch((err) => console.log(err));
  };

  @action addComment = async (values: any) => {
    values.activityId = this.activity!.id;
    try {
      await this.hubConnection!.invoke("SendComment", values);
    } catch (error) {
      console.log(error);
    }
  };

  @computed get activitiesByDate() {
    return this.groupActivitiesByDate(
      Array.from(this.activityRegistry.values())
    );
  }

  // Helper function
  groupActivitiesByDate(activities: IActivity[]) {
    const sortedActivities = activities.sort(
      (a, b) => a.date.getTime() - b.date.getTime()
    );

    return Object.entries(
      // activities is an accumulator - final result that will be returned,
      // activity - current value that is being processed.
      // ---
      // reduce takes a callback (activities...) and and an initVal - initVal is
      // an initial value of an object that will be returned, an empty object
      // with a string key and a value of empty array.
      // ---
      // At the start, there is an empty activity array, that will be taking as
      // elements pairs of keys (date) and values (Array of Activities).
      // reduce() acts on each element of sortedActivities so:
      // it creates a 'date' key based on current element,
      // It checks if activities.date exist via ternary operator '?' -
      // if yes, new activity is being added at the end of current array of activities
      // under date key (spread operator),
      // if not, new activity is being added as the first element of that array
      // under date key.
      // ---
      // reduce returns an object that contains these pairs of key-values - they
      // are passed to Object.entries and are turned into an array.
      sortedActivities.reduce((activities, activity) => {
        // Here I take the date and omit the hours/minutes/etc.
        const date = activity.date.toISOString().split("T")[0];
        activities[date] = activities[date]
          ? [...activities[date], activity]
          : [activity];
        return activities;
        // This is an empty object pass as an initial value but it will containt a key
        // and a value of IActivity array type.
      }, {} as { [key: string]: IActivity[] })
    );
  }

  @action loadActivities = async () => {
    this.loadingInitial = true;
    try {
      // I use async/await but in ts/js it's really the same as promise/lend.
      const activitiesEnvelope = await agent.Activities.list(this.axiosParams);
      const { activities, activityCount } = activitiesEnvelope;
      // This runInAction is a wrapper for nested functions containing state
      // mutations.
      runInAction(() => {
        activities.forEach((activity) => {
          // this.rootStore.userStore.user! - gets current user.
          setActivityProps(activity, this.rootStore.userStore.user!);
          // .set() method takes a key and a value as a parameters.
          // In that case, it will be and id of activity and an activity
          // respectively.
          this.activityRegistry.set(activity.id, activity);
        });
        this.activityCount = activityCount;
        this.loadingInitial = false;
      });
    } catch (error) {
      runInAction(() => {
        this.loadingInitial = false;
      });
      console.log(error);
    }
  };

  // Action for getting a single activity for a 'View' button.
  // This solution have to take into account two situations:
  // 1) User clicks the 'View' button,
  // 2a) User saves that activity View in bookmarks,
  // 2b) User refreshed the page while in View.
  @action loadActivity = async (id: string) => {
    let activity = this.getActivity(id);
    // checks if activity has been returned - if undefined has been returned,
    // then said activity has to be fetched from db.
    if (activity) {
      this.activity = activity;
      // we are taking that activity from registry, where it is an observable,
      // and in case of taking that activity from DB (try block) it is just a plain
      // JS object. The fact that an object is observable and is modified somehow 
      // may cause issues, (we are fixing a bug that happens during editing an Event in safari)
      // So here it's transformed to simple JS object.
      return toJS(activity);
    } else {
      this.loadingInitial = true;
      try {
        activity = await agent.Activities.details(id);
        runInAction(() => {
          setActivityProps(activity, this.rootStore.userStore.user!);
          this.activity = activity;
          this.activityRegistry.set(activity.id, activity);
          this.loadingInitial = false;
        });
        return activity;
      } catch (error) {
        runInAction(() => {
          this.loadingInitial = false;
        });
        console.log(error);
      }
    }
  };

  // function to empty an activity pointer if I change page from Edit form
  // to Create form.
  @action clearActivity = () => {
    this.activity = null;
  };

  // Helper method for loadActivity action.
  // It does not need to be an action because it does not
  // mutate the state.
  getActivity = (id: string) => {
    // returns the value of a given key or undefined.
    return this.activityRegistry.get(id);
  };

  @action createActivity = async (activity: IActivity) => {
    this.submitting = true;
    try {
      // everything will be generated in forms (id too thanks to uuid), so I am
      // just creating a request of adding an activity to db.
      await agent.Activities.create(activity);

      // after creating an activity on server side, this code below
      // replicates what was generated on a server side so it can be immediately
      // seen on client side.
      const attendee = createAttendee(this.rootStore.userStore.user!);

      // due to the fact that I am only updating it on client side, these do not
      // need to be un runInAction.
      attendee.isHost = true;
      let attendees = [];
      attendees.push(attendee);
      activity.attendees = attendees;
      activity.comments = [];
      activity.isHost = true;
      runInAction(() => {
        this.activityRegistry.set(activity.id, activity);
        this.submitting = false;
      });
      history.push(`/activities/${activity.id}`);
    } catch (error) {
      runInAction(() => {
        this.submitting = false;
      });
      toast.error("Problem submitting data");
      console.log(error);
    }
  };

  @action editActivity = async (activity: IActivity) => {
    this.submitting = true;
    try {
      await agent.Activities.update(activity);
      runInAction(() => {
        this.activityRegistry.set(activity.id, activity);
        this.activity = activity;
        this.submitting = false;
      });
      history.push(`/activities/${activity.id}`);
    } catch (error) {
      runInAction(() => {
        this.submitting = false;
      });
      toast.error("Problem submitting data");
      console.log(error);
    }
  };

  @action deleteActivity = async (
    event: SyntheticEvent<HTMLButtonElement>,
    id: string
  ) => {
    this.submitting = true;
    this.target = event.currentTarget.name;
    try {
      await agent.Activities.delete(id);
      runInAction(() => {
        this.activityRegistry.delete(id);
        this.submitting = false;
        this.target = "";
      });
    } catch (error) {
      runInAction(() => {
        this.submitting = false;
        this.target = "";
      });
      console.log(error);
    }
  };

  @action attendActivity = async () => {
    const attendee = createAttendee(this.rootStore.userStore.user!);
    this.loading = true;
    try {
      await agent.Activities.attend(this.activity!.id);
      runInAction(() => {
        if (this.activity) {
          this.activity.attendees.push(attendee);
          this.activity.isGoing = true;
          this.activityRegistry.set(this.activity.id, this.activity);
          this.loading = false;
        }
      });
    } catch (error) {
      runInAction(() => {
        this.loading = false;
      });
      toast.error("Problem signing up to activity");
    }
  };

  @action cancelAttendance = async () => {
    this.loading = true;
    try {
      await agent.Activities.unattend(this.activity!.id);
      runInAction(() => {
        if (this.activity) {
          // this returns a list of attendees apart from currently logged in user.
          this.activity.attendees = this.activity.attendees.filter(
            (a) => a.username !== this.rootStore.userStore.user!.username
          );
          this.activity.isGoing = false;
          this.activityRegistry.set(this.activity.id, this.activity);
          this.loading = false;
        }
      });
    } catch (error) {
      runInAction(() => {
        this.loading = false;
      });
      toast.error("Problem canceling attendance");
    }
  };
}
