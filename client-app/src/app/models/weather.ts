import { IPhoto } from "./profile";

export interface ISpot {
  id: string,
  country: string,
  latitude: string,
  longitude: string,
  name: string,
  imageUrl: string,
  bio: string,
  photos: IPhoto[],
}

export interface IWeather {
  airTemperature: string,
  waterTemperature: string,
  windSpeed: string,
  windAngle: string,
  cloudiness: string,
  tideHeight: string,
}

export interface ISpotPhoto {
  id: string,
  url: string
}