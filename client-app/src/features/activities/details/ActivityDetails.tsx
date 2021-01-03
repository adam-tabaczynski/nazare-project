import { observer } from "mobx-react-lite";
import React, { useContext, useEffect } from "react";
import { RouteComponentProps } from "react-router-dom";
import { Grid, GridColumn } from "semantic-ui-react";
import { LoadingComponent } from "../../../app/layout/LoadingComponent";
import { RootStoreContext } from "../../../app/stores/rootStore";
import ActivityDetailedChat from "./ActivityDetailedChat";
import ActivityDetailedHeader from "./ActivityDetailedHeader";
import ActivityDetailedInfo from "./ActivityDetailedInfo";
import ActivityDetailedSidebar from "./ActivityDetailedSidebar";

// Required for getting into match.params.id
interface DetailParams {
  id: string;
}

// match is being used because it contains an info about activity's id.
// RouComProps take additional parameter - it is required because match.params
// has no knowledge about that "id". We called it that way, but there is no shared
// knowledge of it so I have to explicitly point it
const ActivityDetails: React.FC<RouteComponentProps<DetailParams>> = ({
  match,
  history,
}) => {
  const rootStore = useContext(RootStoreContext);
  const { activity, loadActivity, loadingInitial } = rootStore.activityStore;

  // I get that activity in case of user saved that View of activity in his
  // bookmarks or he refreshed the page. This has to be run only once, so
  // I pass the array of dependency. Other case, this would run every time
  // the component rerenders.
  useEffect(() => {
    loadActivity(match.params.id);
  }, [loadActivity, match.params.id, history]);

  if (loadingInitial) {
    return <LoadingComponent content="Trwa ładowanie..." />;
  }

  if (!activity) {
    return <h2>Nie znaleziono wydarzenia</h2>;
  }

  return (
    <Grid>
      <GridColumn width={10}>
        <ActivityDetailedHeader activity={activity} />
        <ActivityDetailedInfo activity={activity} />
        <ActivityDetailedChat />
      </GridColumn>
      <GridColumn width={6}>
        <ActivityDetailedSidebar attendees={activity.attendees}/>
      </GridColumn>
    </Grid>
  );
};

export default observer(ActivityDetails);
