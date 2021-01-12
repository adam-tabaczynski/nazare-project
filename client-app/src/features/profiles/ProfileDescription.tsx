import React, { useContext, useState } from "react";
import { Tab, Grid, Header, Button, Segment } from "semantic-ui-react";
import { RootStoreContext } from "../../app/stores/rootStore";
import ProfileEditForm from "./ProfileEditForm";
import { observer } from "mobx-react-lite";

const ProfileDescription = () => {
  const rootStore = useContext(RootStoreContext);
  const { updateProfile, profile, isCurrentUser } = rootStore.profileStore;
  const [editMode, setEditMode] = useState(false);
  return (
    <Tab.Pane>
      <Grid>
        <Grid.Column width={16}>
          <Header
            floated="left"
            icon="user"
            content={`Opis użytkownika ${profile!.displayName}`}
          />
          {isCurrentUser && (
            <Button
              floated="right"
              basic
              content={editMode ? "Zamknij" : "Edytuj opis"}
              onClick={() => setEditMode(!editMode)}
            />
          )}
        </Grid.Column>
        <Grid.Column width={16}>
          {editMode ? (
            <ProfileEditForm updateProfile={updateProfile} profile={profile!} />
          ) : (
            <Segment raised><p style={{textAlign: "justify"}}>{profile!.bio}</p></Segment>
          )}
        </Grid.Column>
      </Grid>
    </Tab.Pane>
  );
};

export default observer(ProfileDescription);
