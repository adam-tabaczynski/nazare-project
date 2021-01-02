import axios, { AxiosResponse } from "axios";
import { toast } from "react-toastify";
import { history } from "../..";
import { IActivitiesEnvelope, IActivity } from "../models/activity";
import { IPhoto, IProfile } from "../models/profile";
import { IUser, IUserFormValues } from "../models/user";
import { ISpot } from "../models/weather";

axios.defaults.baseURL = process.env.REACT_APP_API_URL;

// For every request being made, this checks if there is a token.
// If there is, said token is attached to Headers Auth (like in Postman),
// and then send further.
axios.interceptors.request.use(
  (config) => {
    const token = window.localStorage.getItem("jwt");
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// first parameter is 'what to do when response is fulfilled,
// second is what to do when error happens.
axios.interceptors.response.use(undefined, (error) => {
  if (error.message === "Network Error" && !error.response) {
    toast.error("Network error - make sure API is running!");
  }
  const { status, data, config, headers } = error.response;
  if (status === 404) {
    history.push("/notfound");
  }
  if (
    status === 401 &&
    headers["www-authenticate"].includes("The token expired")
  ) {
    window.localStorage.removeItem("jwt");
    history.push("/");
    toast.info("Your session has expire, please login again");
  }
  if (
    status === 400 &&
    config.method === "get" &&
    data.errors.hasOwnProperty("id")
  ) {
    history.push("/notfound");
  }
  if (status === 500) {
    toast.error("Server error - check the terminal for more info!");
  }
  throw error.response;
});

// Here I explictly tell the type of response variable.
const responseBody = (response: AxiosResponse) => response.data;

// Requests types.
const requests = {
  get: (url: string) => axios.get(url).then(responseBody),
  post: (url: string, body: {}) => axios.post(url, body).then(responseBody),
  put: (url: string, body: {}) => axios.put(url, body).then(responseBody),
  del: (url: string) => axios.delete(url).then(responseBody),
  postForm: (url: string, file: Blob) => {
    let formData = new FormData();
    formData.append("File", file);
    return axios
      .post(url, formData, {
        headers: { "Content-type": "multipart/form-data" },
      })
      .then(responseBody);
  },
};

// Requests for CRUD application.
const Activities = {
  // axios allows passing parameters to an URL - here I pass these params of
  // URLSearchParams.
  list: (params: URLSearchParams): Promise<IActivitiesEnvelope> =>
    axios
      .get("/activities", { params: params })

      .then(responseBody),
  details: (id: string) => requests.get(`/activities/${id}`),
  create: (activity: IActivity) => requests.post("/activities/", activity),
  update: (activity: IActivity) =>
    requests.put(`/activities/${activity.id}`, activity),
  delete: (id: string) => requests.del(`activities/${id}`),
  // empty object is to omit an error, function wants a body but it will be
  // added on API side, not here.
  attend: (id: string) => requests.post(`/activities/${id}/attend`, {}),
  unattend: (id: string) => requests.del(`/activities/${id}/attend`),
};

const User = {
  current: (): Promise<IUser> => requests.get("/user"),
  login: (user: IUserFormValues): Promise<IUser> =>
    requests.post(`/user/login`, user),
  register: (user: IUserFormValues): Promise<IUser> =>
    requests.post(`/user/register`, user),
};

const Profiles = {
  get: (username: string): Promise<IProfile> =>
    requests.get(`/profiles/${username}`),
  uploadPhoto: (photo: Blob): Promise<IPhoto> =>
    requests.postForm(`/photos`, photo),
  setMainPhoto: (id: string) => requests.post(`/photos/${id}/setMain`, {}),
  deletePhoto: (id: string) => requests.del(`/photos/${id}`),
  follow: (username: string) =>
    requests.post(`/profiles/${username}/follow`, {}),
  unfollow: (username: string) => requests.del(`/profiles/${username}/follow`),
  listFollowings: (username: string, predicate: string) =>
    requests.get(`/profiles/${username}/follow?predicate=${predicate}`),
  listActivities: (username: string, predicate: string) =>
    requests.get(`/profiles/${username}/activities?predicate=${predicate}`),
};

const SpotWeatherRequests = {
  getWeather: (id: string) => requests.get(`/weather/${id}`),
  getSpotsList: (): Promise<ISpot[]> => requests.get("/weather"),
  getSpot: (id: string) => requests.get(`/weather/spot/${id}`),
};

export default {
  Activities,
  User,
  Profiles,
  SpotWeatherRequests,
};
