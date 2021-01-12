import React from "react";
import { Link } from "react-router-dom";
import { Button, Icon, Item, Label, Segment } from "semantic-ui-react";
import { IActivity } from "../../../app/models/activity";
import { format } from "date-fns";
import ActivityListItemAttendees from "./ActivityListItemAttendees";
import { pl } from "date-fns/locale";

const buttonGap = {
  marginTop: "15px",
};

const ActivityListItem: React.FC<{ activity: IActivity }> = ({ activity }) => {
  const host = activity.attendees.filter((x) => x.isHost)[0];
  return (
    <Segment.Group>
      <Segment>
        <Item.Group>
          <Item>
            <Item.Image
              size="tiny"
              circular
              src={host.image || "/assets/user.png"}
              style={{ marginBottom: 3 }}
            />
            <Item.Content>
              <Item.Header as={Link} to={`/activities/${activity.id}`}>
                {activity.title}
              </Item.Header>
              <Item.Description>
                Organizowane przez
                <Link to={`/profile/${host.username}`}>
                  {" "}
                  {host.displayName}
                </Link>{" "}
              </Item.Description>
              {activity.isHost && (
                <Item.Description>
                  <Label basic color="orange" content="Organizujesz" />
                </Item.Description>
              )}
              {activity.isGoing && !activity.isHost && (
                <Item.Description>
                  <Label basic color="green" content="Uczestniczysz" />
                </Item.Description>
              )}
            </Item.Content>
          </Item>
        </Item.Group>
      </Segment>
      <Segment>
        <Icon name="clock" /> {format(activity.date, "H:mm", { locale: pl })}
        <Icon name="marker" /> {activity.venue}, {activity.city}
      </Segment>
      <Segment secondary>
        <ActivityListItemAttendees attendees={activity.attendees} />
      </Segment>
      <Segment clearing>
        <span>{activity.description}</span>
        <div>
          <Button
            as={Link}
            style={buttonGap}
            to={`/activities/${activity.id}`}
            floated="right"
            content="Zobacz"
            color="blue"
          />
        </div>
      </Segment>
    </Segment.Group>
  );
};

export default ActivityListItem;
