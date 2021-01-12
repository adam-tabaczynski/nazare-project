import { action, makeObservable, observable, runInAction } from "mobx";
import agent from "../api/agent";
import { RootStore } from "./rootStore";
import { ISpot, IWeather } from "../models/weather";

export default class WeatherStore {
  rootStore: RootStore;
  constructor(rootStore: RootStore) {
    this.rootStore = rootStore;
    makeObservable(this);
  }

  @observable spotsList: ISpot[] = [];
  @observable weather: IWeather | null = null;
  @observable weatherCardWasClicked = false;
  @observable currentSpot: ISpot | null = null;

  @action getWeather = async (id: string) => {
    try {
      const weather = await agent.SpotWeatherRequests.getWeather(id);
      runInAction(() => {
        this.weather = weather;
      });
    } catch (error) {
      console.log(error);
    }
  };

  @action getSpotsList = async () => {
    try {
      const spotsList = await agent.SpotWeatherRequests.getSpotsList();
      runInAction(() => {
        this.spotsList = spotsList;
      });
    } catch (error) {
      console.log(error);
    }
  };

  @action getSpot = async (id: string) => {
    try {
      const currentSpot = await agent.SpotWeatherRequests.getSpot(id);
      runInAction(() => {
        this.currentSpot = currentSpot;
      });
    } catch (error) {
      console.log(error);
    }
  };

  @action getWeatherAndSpot = async (id: string) => {
    this.weatherCardWasClicked = true;
    this.getSpot(id);
    this.getWeather(id);
  };
}
