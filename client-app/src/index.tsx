import React from "react";
import ReactDOM from "react-dom";
import "./app/layout/styles.css";
import App from "./app/layout/App";
import 'react-toastify/dist/ReactToastify.min.css';
import 'react-widgets/dist/css/react-widgets.css';
import * as serviceWorker from "./serviceWorker";
import { Router } from "react-router-dom";
import { createBrowserHistory } from "history";
import ScrollToTop from "./app/layout/ScrollToTop";
import dateFnsLocalizer from 'react-widgets-date-fns';

// Used for date and time widget in Form
dateFnsLocalizer();

// This is required for redirecting users in case of 404 code response.
export const history = createBrowserHistory();

// Renders an <App /> component inside div root in index.html.
ReactDOM.render(
    <Router history={history}>
      <ScrollToTop>
        <App />
      </ScrollToTop>
    </Router>,
  document.getElementById("root")
);

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister();