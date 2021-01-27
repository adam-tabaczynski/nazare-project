import React, { Fragment, useContext } from "react";
import { Segment } from "semantic-ui-react";
import { RootStoreContext } from "../../app/stores/rootStore";

const divStyle = {
  display: "flex",
  justifyContent: "center",
};

const ProfileMovies = () => {

  const rootStore = useContext(RootStoreContext);
  const {
    profile
  } = rootStore.profileStore;

  return (
    <Fragment>
      <Segment>
        <div style={divStyle}>
          <iframe
            title="1"
            width="480"
            height="360"
            src={profile?.movie}
            frameBorder="0"
            allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
            allowFullScreen
          ></iframe>
        </div>
      </Segment>
    </Fragment>
  );
};

export default ProfileMovies;
