import React from "react";
import { IProfile } from "../../app/models/profile";
import { Form as FinalForm, Field } from "react-final-form";
import { observer } from "mobx-react-lite";
import { combineValidators, isRequired } from "revalidate";
import { Form, Button, Input, Grid } from "semantic-ui-react";
import TextInput from "../../app/common/form/TextInput";

const validate = combineValidators({
  displayName: isRequired("displayName"),
});

interface IProps {
  updateMovie: (profile: Partial<IProfile>) => void;
  profile: IProfile;
}

const ProfileMovieEditForm: React.FC<IProps> = ({ updateMovie, profile }) => {
  return (
    <FinalForm
      onSubmit={updateMovie}
      validate={validate}
      initialValues={profile!}
      render={({ handleSubmit, invalid, pristine, submitting }) => (
        <Grid.Column>
          <Form onSubmit={handleSubmit} error>
            <label>Link do filmu</label>
            <Field
              name="movie"
              component={TextInput}
              placeholder="Link do filmu"
              value={profile!.displayName}
            />
            <Button
              loading={submitting}
              floated="right"
              disabled={invalid || pristine}
              positive
              content="Ustaw"
            />
          </Form>
        </Grid.Column>
      )}
    />
  );
};

export default observer(ProfileMovieEditForm);
