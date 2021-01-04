import { FORM_ERROR } from "final-form";
import React, { useContext } from "react";
import { Form as FinalForm, Field } from "react-final-form";
import { combineValidators, isRequired } from "revalidate";
import { Button, Form, Header } from "semantic-ui-react";
import ErrorMessage from "../../app/common/form/ErrorMessage";
import TextInput from "../../app/common/form/TextInput";
import { IUserFormValues } from "../../app/models/user";
import { RootStoreContext } from "../../app/stores/rootStore";

const validate = combineValidators({
  email: isRequired("email"),
  password: isRequired("password"),
});

const LoginForm = () => {
  const rootStore = useContext(RootStoreContext);
  const { login } = rootStore.userStore;
  return (
    <FinalForm
      onSubmit={(values: IUserFormValues) =>
        login(values).catch((error) => ({
          // Error from form will be stored in that FORM_ERROR
          [FORM_ERROR]: error,
        }))
      }
      validate={validate}
      render={({
        handleSubmit,
        submitting,
        form,
        submitError,
        invalid,
        pristine,
        dirtySinceLastSubmit,
      }) => (
        <Form onSubmit={handleSubmit} error>
          <Header
            as="h2"
            content="Zaloguj się"
            color="teal"
            textAlign="center"
          />
          <Field name="email" component={TextInput} placeholder="Adres e-mail" />
          <Field
            name="password"
            component={TextInput}
            placeholder="Hasło"
            type="password"
          />
          {submitError && !dirtySinceLastSubmit && (
            <ErrorMessage
              error={submitError}
              text="Niepoprawny e-mail lub hasło"
            />
          )}
          <br />
          <Button
            disabled={(invalid && !dirtySinceLastSubmit) || pristine}
            loading={submitting}
            color="teal"
            content="Zaloguj się"
            fluid
          />
        </Form>
      )}
    />
  );
};

export default LoginForm;
