import React, { useContext, useEffect, useState } from "react";
import { Button, Form, Grid, Segment } from "semantic-ui-react";
import { ActivityFormValues } from "../../../app/models/activity";
import { observer } from "mobx-react-lite";
import { RouteComponentProps } from "react-router-dom";
import { Form as FinalForm, Field } from "react-final-form";
import TextInput from "../../../app/common/form/TextInput";
import TextAreaInput from "../../../app/common/form/TextAreaInput";
import SelectInput from "../../../app/common/form/SelectInput";
import { category } from "../../../app/common/options/categoryOptions";
import DateInput from "../../../app/common/form/DateInput";
import { combineDateAndTime } from "../../../app/common/util/util";
import {
  combineValidators,
  composeValidators,
  hasLengthGreaterThan,
  isRequired,
} from "revalidate";
import { v4 as uuid } from "uuid";
import { RootStoreContext } from "../../../app/stores/rootStore";

const validate = combineValidators({
  title: isRequired({ message: "Podanie tytułu wydarzenia jest obowiązkowe" }),
  category: isRequired({ message: "Podanie kategorii wydarzenia jest obowiązkowe" }),
  description: composeValidators(
    isRequired({message: "Wpisanie opisu wydarzenia jest obowiązkowe"}),
    hasLengthGreaterThan(4)({
      message: "Opis musi mieć conajmniej 5 znaków",
    })
  )(),
  city: isRequired({ message: "Podanie miasta jest obowiązkowe" }),
  venue: isRequired({ message: "Podanie adresu jest obowiązkowe" }),
  date: isRequired({ message: "Podanie dnia wydarzenia jest obowiązkowe" }),
  time: isRequired({ message: "Podanie godziny wydarzenia jest obowiązkowe" }),
});

interface DetailParams {
  id: string;
}

const ActivityForm: React.FC<RouteComponentProps<DetailParams>> = ({
  match,
  history,
}) => {
  const rootStore = useContext(RootStoreContext);
  const {
    submitting,
    loadActivity,
    createActivity,
    editActivity,
  } = rootStore.activityStore;

  // in activity.ts there is an ActivityFormValues constructor that initiate
  // the empty activity if no params are passed.
  const [activity, setActivity] = useState(new ActivityFormValues());
  const [loading, setLoading] = useState(false);

  // This runs every single time the component rerenders - I want to run it
  // only when I am editing an activity.
  // I am checking lenght of activity.id because sole match.params.id check
  // generates error after page is redirected out of Form.
  // If there is no form, the activity.id is 0 so after redirecting, that
  // if() will not be called again.
  useEffect(() => {
    if (match.params.id) {
      setLoading(true);
      // loadActivity returns Promise and this allows using .then().
      loadActivity(match.params.id)
        .then((activity) => setActivity(new ActivityFormValues(activity)))
        .finally(() => setLoading(false));
    }
    // Dependencies here works in a way that the useEffect will run only if any
    // of these four dependencies change.
  }, [loadActivity, match.params.id]);

  const handleFinalFormSubmit = (values: any) => {
    const dateAndTime = combineDateAndTime(values.date, values.time);
    const { date, time, ...activity } = values;
    activity.date = dateAndTime;
    if (!activity.id) {
      let newActivity = {
        ...activity,
        //  This works because i installed npm uuid, check notes
        id: uuid(),
      };

      createActivity(newActivity);
    } else {
      editActivity(activity);
    }
  };

  return (
    <Grid>
      <Grid.Column width={10}>
        <Segment clearing>
          <FinalForm
            validate={validate}
            initialValues={activity}
            onSubmit={handleFinalFormSubmit}
            render={({ handleSubmit, invalid, pristine }) => (
              <Form onSubmit={handleSubmit} loading={loading}>
                <Field
                  name="title"
                  placeholder="Tytuł"
                  value={activity.title}
                  component={TextInput}
                />
                <Field
                  name="description"
                  placeholder="Opis"
                  rows={3}
                  value={activity.description}
                  component={TextAreaInput}
                />
                <Field
                  name="category"
                  placeholder="Kategoria"
                  options={category}
                  value={activity.category}
                  component={SelectInput}
                />
                <Form.Group widths="equal">
                  <Field
                    name="date"
                    date={true}
                    placeholder="Data"
                    value={activity.date}
                    component={DateInput}
                  />
                  <Field
                    name="time"
                    time={true}
                    placeholder="Godzina"
                    value={activity.time}
                    component={DateInput}
                  />
                </Form.Group>
                <Field
                  name="city"
                  placeholder="Miasto"
                  value={activity.city}
                  component={TextInput}
                />
                <Field
                  name="venue"
                  placeholder="Adres"
                  value={activity.venue}
                  component={TextInput}
                />
                <Button
                  loading={submitting}
                  disabled={loading || invalid || pristine}
                  floated="right"
                  positive
                  type="submit"
                  content="Zatwierdź"
                />
                <Button
                  onClick={
                    activity.id
                      ? () => history.push(`/activities/${activity.id}`)
                      : () => history.push("/activities")
                  }
                  disabled={loading}
                  floated="right"
                  type="submit"
                  content="Anuluj"
                />
              </Form>
            )}
          />
        </Segment>
      </Grid.Column>
    </Grid>
  );
};

export default observer(ActivityForm);
