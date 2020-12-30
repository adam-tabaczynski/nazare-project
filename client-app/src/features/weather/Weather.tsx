import { observer } from "mobx-react-lite";
import { format } from "path";
import React, { Fragment, useContext, useEffect } from "react";
import { Link } from "react-router-dom";
import { Card, Image, Segment } from "semantic-ui-react";
import { IUserActivity } from "../../app/models/profile";
import { ISpot } from "../../app/models/weather";
import { RootStoreContext } from "../../app/stores/rootStore";

const Weather = () => {
  const rootStore = useContext(RootStoreContext);
  const {
    getSpotsList,
    spotsList,
  } = rootStore.weatherStore;

  useEffect(() => {
    getSpotsList();
  }, [getSpotsList]);
  return (

      <Segment>
        <Card.Group itemsPerRow={5}>
          {spotsList.map((spot: ISpot) => (
            <Card>
              {console.log("KartaUtwprzona")}
              <Image
                src={
                  "https://media-cdn.tripadvisor.com/media/photo-s/18/57/8a/a2/copacabana.jpg"
                }
              />
              <Card.Content>
                <Card.Header textAlign="center">
                  <div>Copacabana</div>
                </Card.Header>
                <Card.Meta textAlign="center">
                  <div>Lat</div>
                  <div>Long</div>
                </Card.Meta>
              </Card.Content>
            </Card>
          ))}
        </Card.Group>
      </Segment>
  );
};

export default observer(Weather);