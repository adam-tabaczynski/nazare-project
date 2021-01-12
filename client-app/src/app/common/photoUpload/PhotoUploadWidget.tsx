import React, { Fragment, useEffect, useState } from "react";
import { Header, Grid, Button } from "semantic-ui-react";
import { observer } from "mobx-react-lite";
import PhotoWidgetDropzone from "./PhotoWidgetDropzone";
import PhotoWidgetCropper from "./PhotoWidgetCropper";

interface IProps {
  loading: boolean;
  uploadPhoto: (file: Blob) => void;
}

const PhotoUploadWidget: React.FC<IProps> = ({ loading, uploadPhoto }) => {
  // This will be required to show image preview in step3 of Photos tab in user profile.
  const [files, setFiles] = useState<any[]>([]);

  const [image, setImage] = useState<Blob | null>(null);

  useEffect(() => {
    // This will cleanup the image from preview step 3 to prevent memory leak
    // if user decide to go to another tab.
    return () => {
      files.forEach((file) => URL.revokeObjectURL(file.preview));
    };
  });

  return (
    <Fragment>
      <Grid>
        <Grid.Column width={4}>
          <Header color="teal" sub content="Krok 1 - Dodaj zdjęcie" />
          <PhotoWidgetDropzone setFiles={setFiles} />
        </Grid.Column>
        <Grid.Column width={1} />
        <Grid.Column width={4}>
          <Header sub color="teal" content="Krok 2 - Określ wymiary zdjęcia" />
          {files.length > 0 && (
            <PhotoWidgetCropper
              setImage={setImage}
              imagePreview={files[0].preview}
            />
          )}
        </Grid.Column>
        <Grid.Column width={1} />
        <Grid.Column width={4}>
          <Header sub color="teal" content="Krok 3 - Podgląd zdjęcia" />
          {files.length > 0 && (
            <Fragment>
              <div
                className="img-preview"
                style={{ minHeight: "200px", overflow: "hidden" }}
              />
              <Button.Group widths={2}>
                <Button
                  positive
                  icon="check"
                  loading={loading}
                  onClick={() => uploadPhoto(image!)}
                />
                <Button
                  icon="close"
                  disabled={loading}
                  onClick={() => setFiles([])}
                />
              </Button.Group>
            </Fragment>
          )}
        </Grid.Column>
      </Grid>
    </Fragment>
  );
};

export default observer(PhotoUploadWidget);
