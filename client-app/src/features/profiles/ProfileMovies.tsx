import React, { useContext, useState } from "react";
import { Button, Grid, Header, Tab } from "semantic-ui-react";
import { RootStoreContext } from "../../app/stores/rootStore";
import ProfileMovieEditForm from "./ProfileMovieEditForm";



const ProfileMovies = () => {
  const rootStore = useContext(RootStoreContext);
  const { isCurrentUser, profile, updateMovie } = rootStore.profileStore;

  const [addMovieMode, setAddMovieMode] = useState(false);

  return (
    <Tab.Pane>
      <Grid>
        <Grid.Column width={16} style={{ paddingBottom: 0 }}>
          <Header floated="left" icon="film" content="Film" />
          {isCurrentUser && (
            <Button
              floated="right"
              basic
              content={addMovieMode ? "Anuluj" : "Dodaj film"}
              onClick={() => setAddMovieMode(!addMovieMode)}
            />
          )}
        </Grid.Column>
        {addMovieMode ? (
          <Grid.Row centered style={{ marginTop: 40}}>
            <ProfileMovieEditForm updateMovie={updateMovie} profile={profile!} />
            {/* <Input style={divStyle} size={"large"} label={"Link"} icon='youtube' placeholder='Tutaj wklej link filmu' /> */}
          </Grid.Row>
        ) : (
          <Grid.Row centered>
            <iframe
              title="1"
              width="480"
              height="360"
              src={profile?.movie}
              frameBorder="0"
              allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
              allowFullScreen
            ></iframe>
          </Grid.Row>
        )}
      </Grid>
    </Tab.Pane>
  );
};

export default ProfileMovies;
