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
            <Table.Cell>Temperatura powietrza</Table.Cell>
            <Table.Cell>{weather?.airTemperature} {weather?.airTemperature && "°C"}</Table.Cell>
          </Table.Row>
          <Table.Row>
            <Table.Cell>Temperatura wody</Table.Cell>
            <Table.Cell>{weather?.waterTemperature} {weather?.waterTemperature && "°C"}</Table.Cell>
          </Table.Row>
          <Table.Row>
            <Table.Cell>Prędkość wiatru</Table.Cell>
            <Table.Cell>{weather?.windSpeed} {weather?.windSpeed && "m/s"}</Table.Cell>
          </Table.Row>

          <Table.Row>
            <Table.Cell>Kierunek wiatru</Table.Cell>
            <Table.Cell>{weather?.windAngle}</Table.Cell>
          </Table.Row>

          <Table.Row>
            <Table.Cell>Zachmurzenie</Table.Cell>
            <Table.Cell>{weather?.cloudiness}{weather?.cloudiness && "%"}</Table.Cell>
          </Table.Row>

          <Table.Row>
            <Table.Cell>Wysokośc fali</Table.Cell>
            <Table.Cell>{weather?.tideHeight}{weather?.tideHeight && "m"}</Table.Cell>
          </Table.Row>
        </Table.Body>
      </Table>
    </Fragment>
  );
};

export default observer(Weather);
