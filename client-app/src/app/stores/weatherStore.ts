import { action, makeObservable, observable, runInAction } from "mobx";
import agent from "../api/agent";
import { RootStore } from "./rootStore";
import { ISpot } from "../models/weather";

export default class WeatherStore {
  rootStore: RootStore;
  constructor(rootStore: RootStore) {
    this.rootStore = rootStore;
    makeObservable(this);
  }

  @observable spotsList: ISpot[] = [];
  @observable weather: ISpot | null = null;

  @action getWeather = async (id: string) => {
    try {
      const weather = await agent.SpotWeatherRequests.getWeather(id);
      runInAction(() => {
        this.weather = weather;
      })
    } catch (error) {
      console.log(error);
    }
  };

  @action getSpotsList = async () => {
    try {
      const spotsList = await agent.SpotWeatherRequests.getSpotsList();
      runInAction(() => {
        
        this.spotsList = spotsList;
      })
    } catch (error) {
      console.log(error);
    }
  }
}
