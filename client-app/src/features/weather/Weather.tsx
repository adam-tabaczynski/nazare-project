import { observer } from "mobx-react-lite";
import React, { Fragment, useContext, useEffect } from "react";
import {
  Card,
  Grid,
  GridColumn,
  Header,
  Image,
  Segment,
  Table,
} from "semantic-ui-react";
import { ISpot } from "../../app/models/weather";
import { RootStoreContext } from "../../app/stores/rootStore";


const descriptionSegmentStyle = {
  height: "298px",
  marginBottom: "20px"!,
};
const photoSegmentStyle = {
  height: "350px",
  marginBottom: "20px"!,
};

const imgStyle = {
  height: "280px",
  width: "360px",
};

const Weather = () => {
  const rootStore = useContext(RootStoreContext);
  const {
    getSpotsList,
    spotsList,
    getWeatherAndSpot,
    currentSpot,
    weather,
    weatherCardWasClicked,
  } = rootStore.weatherStore;

  useEffect(() => {
    getSpotsList();
  }, [getSpotsList]);
  return (
    <Fragment>
      <Card.Group itemsPerRow={5}>
        {spotsList.map((spot: ISpot) => (
          <Card key={spot.id} onClick={() => getWeatherAndSpot(spot.id)}>
            <Image src={spot.imageUrl} />
            <Card.Content>
              <Card.Header textAlign="center">
                <div>{spot.name},</div>
                <div>{spot.country}</div>
              </Card.Header>
              <Card.Meta textAlign="center"></Card.Meta>
            </Card.Content>
          </Card>
        ))}
      </Card.Group>
      {weatherCardWasClicked && (
        <Fragment>
          <Grid centered>
            <Grid.Column width={8}>
              <Table celled fixed singleLine>
                <Table.Header>
                  <Table.Row>
                    <Table.HeaderCell>
                      Nazwa Spotu: {currentSpot?.name}
                    </Table.HeaderCell>
                    <Table.HeaderCell>Wartość</Table.HeaderCell>
                  </Table.Row>
                </Table.Header>

                <Table.Body>
                  <Table.Row>
                    <Table.Cell>Temperatura powietrza</Table.Cell>
                    <Table.Cell>
                      {weather?.airTemperature}{" "}
                      {!!weather?.airTemperature && "°C"}
                    </Table.Cell>
                  </Table.Row>
                  <Table.Row>
                    <Table.Cell>Temperatura wody</Table.Cell>
                    <Table.Cell>
                      {weather?.waterTemperature}{" "}
                      {!!weather?.waterTemperature && "°C"}
                    </Table.Cell>
                  </Table.Row>
                  <Table.Row>
                    <Table.Cell>Prędkość wiatru</Table.Cell>
                    <Table.Cell>
                      {weather?.windSpeed} {weather?.windSpeed && "m/s"}
                    </Table.Cell>
                  </Table.Row>

                  <Table.Row>
                    <Table.Cell>Kierunek wiatru</Table.Cell>
                    <Table.Cell>{weather?.windAngle}</Table.Cell>
                  </Table.Row>

                  <Table.Row>
                    <Table.Cell>Zachmurzenie</Table.Cell>
                    <Table.Cell>
                      {weather?.cloudiness} {!!weather?.cloudiness && "%"}
                    </Table.Cell>
                  </Table.Row>

                  <Table.Row>
                    <Table.Cell>Wysokość fali</Table.Cell>
                    <Table.Cell>
                      {weather?.tideHeight} {weather?.tideHeight && "m"}
                    </Table.Cell>
                  </Table.Row>
                </Table.Body>
              </Table>
            </Grid.Column>
            <Grid.Column width={8}>
              <Segment style={descriptionSegmentStyle}>
                <Header icon="pencil alternate" content={`Opis`} />
                {/* <span>{currentSpot?.bio}</span> */}
                <span>
                  Jedna z najbardziej znanych miejscowości na windsurfingowej i
                  kitesurfingowej mapie Polski - duże imprezy sportowe,
                  niepowtarzalny klimat oraz dobre warunki pogodowe w trakcie
                  lata powodują, że jest to miejsce do którego chętnie kierują
                  się osoby spragnione adrenaliny i rozrywki.
                </span>
              </Segment>
            </Grid.Column>
          </Grid>

          <Segment style={photoSegmentStyle}>
            <Header icon="camera" content={`Zdjęcia`} />
            <Image.Group >
              <Image style={imgStyle} src={"/assets/stockJastarnia/noc.jpg"}></Image>
              <Image style={imgStyle} src={"/assets/stockJastarnia/widok.jpg"}></Image>
              <Image style={imgStyle} src={"/assets/stockJastarnia/windsurf.jpg"}></Image>
            </Image.Group>
          </Segment>
          <br />
        </Fragment>
      )}
    </Fragment>
  );
};

export default observer(Weather);
