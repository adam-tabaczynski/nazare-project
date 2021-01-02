import { observer } from "mobx-react-lite";
import React, { Fragment, useContext, useEffect } from "react";
import { Card, Image, Table } from "semantic-ui-react";
import { ISpot } from "../../app/models/weather";
import { RootStoreContext } from "../../app/stores/rootStore";

const Weather = () => {
  const rootStore = useContext(RootStoreContext);
  const {
    getSpotsList,
    spotsList,
    getWeatherAndSpot,
    currentSpot,
    weather,
  } = rootStore.weatherStore;

  useEffect(() => {
    getSpotsList();
  }, [getSpotsList]);
  return (
    <Fragment>
      <Card.Group itemsPerRow={5}>
        {spotsList.map((spot: ISpot) => (
          <Card key={spot.id} onClick={() => getWeatherAndSpot(spot.id)}>
            <Image
              src={
                "https://media-cdn.tripadvisor.com/media/photo-s/18/57/8a/a2/copacabana.jpg"
              }
            />
            <Card.Content>
              <Card.Header textAlign="center">
                <div>{spot.name}</div>
              </Card.Header>
              <Card.Meta textAlign="center">
              </Card.Meta>
            </Card.Content>
          </Card>
        ))}
      </Card.Group>
      <Table celled fixed singleLine>
        <Table.Header>
          <Table.Row>
            <Table.HeaderCell>Nazwa Spotu: {currentSpot?.name}</Table.HeaderCell>
            <Table.HeaderCell>Wartość</Table.HeaderCell>
          </Table.Row>
        </Table.Header>

        <Table.Body>
          <Table.Row>
            <Table.Cell>Wind Speed</Table.Cell>
            <Table.Cell>{weather?.windSpeed}</Table.Cell>
          </Table.Row>

          <Table.Row>
            <Table.Cell>Wind angle</Table.Cell>
            <Table.Cell>{weather?.windAngle}</Table.Cell>
          </Table.Row>

          <Table.Row>
            <Table.Cell>Air temperature</Table.Cell>
            <Table.Cell>{weather?.airTemperature}</Table.Cell>
          </Table.Row>

          <Table.Row>
            <Table.Cell>Cloudiness</Table.Cell>
            <Table.Cell>{weather?.cloudiness}</Table.Cell>
          </Table.Row>
        </Table.Body>
      </Table>
    </Fragment>
  );
};

export default observer(Weather);
