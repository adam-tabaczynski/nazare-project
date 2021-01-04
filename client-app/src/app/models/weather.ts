export interface ISpot {
  id: string,
  country: string,
  latitude: string,
  longitude: string,
  name: string,
  imageUrl: string,
  bio: string,
}

export interface IWeather {
  airTemperature: string,
  waterTemperature: string,
  windSpeed: string,
  windAngle: string,
  cloudiness: string,
  tideHeight: string,
}