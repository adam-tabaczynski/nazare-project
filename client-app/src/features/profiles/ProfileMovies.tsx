import React, { Fragment } from "react";
import { Segment } from "semantic-ui-react";

const divStyle = {
  display: "flex",
  justifyContent: "center",
};

const ProfileMovies = () => {
  return (
    <Fragment>
      <Segment>
        <div style={divStyle}>
          <iframe
            title="1"
            width="480"
            height="360"
            src="https://www.youtube.com/embed/xB0lb71GSFk"
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
