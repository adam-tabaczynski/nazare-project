import React from "react";
import { FieldRenderProps } from "react-final-form";
import { Form, FormFieldProps, Label } from "semantic-ui-react";
import { DateTimePicker } from 'react-widgets';
import dateFnsLocalizer, { defaultFormats } from "react-widgets-date-fns";
import pl from "date-fns/locale/pl";

interface IProps
  extends FieldRenderProps<Date, HTMLInputElement>,
    FormFieldProps {}

const DateInput: React.FC<IProps> = ({
  input,
  width,
  placeholder,
  meta: { touched, error },
  date = false,
  time = false,
  ...rest
}) => {
  const formats = Object.assign(defaultFormats, { default: "mmm YY" });
  dateFnsLocalizer({ formats, locales: { 'pl': pl } });
  
  return (
    <Form.Field error={touched && !!error} width={width}>
      <DateTimePicker
        culture={"pl"}
        placeholder={placeholder}
        value={input.value || null}
        onChange={input.onChange}
        onBlur={input.onBlur}
        onKeyDown={(e) => e.preventDefault()}
        date={date}
        time={time}
        {...rest}
        />

      {touched && error && (
        <Label basic color="red">
          {error}
        </Label>
      )}
    </Form.Field>
  );
};

export default DateInput;
