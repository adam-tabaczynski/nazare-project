import { observer } from "mobx-react-lite";
import React, { Fragment, useContext } from "react";
import { Item, Label } from "semantic-ui-react";
import { RootStoreContext } from "../../../app/stores/rootStore";
import ActivityListItem from "./ActivityListItem";
import { format } from 'date-fns';

// Very similiar stuff to ActivityDashboard - key has been added to Item,
// because each iteration of .map on activities will generate new Item.
// activites are available w/o 'props.' because props interface has been
// specified in parameters and activites has been passed explicitly in
// curly brackets as an arg.
const ActivityList: React.FC = () => {
  const rootStore = useContext(RootStoreContext);
  const { activitiesByDate } = rootStore.activityStore;

  return (
    <Fragment>
      {activitiesByDate.map(([group, activities]) => (
        <Fragment key={group}>
          <Label size="large" color="blue">
            {format(group, 'eeee do MMMM')}
          </Label>
            <Item.Group divided>
              {activities.map((activity) => (
                <ActivityListItem key={activity.id} activity={activity} />
              ))}
            </Item.Group>
        </Fragment>
      ))}
    </Fragment>
  )
};

export default observer(ActivityList);
