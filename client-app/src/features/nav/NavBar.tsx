import { observer } from "mobx-react-lite";
import React, { useContext } from "react";
import { Link, NavLink } from "react-router-dom";
import { Button, Container, Dropdown, Image, Menu } from "semantic-ui-react";
import { RootStoreContext } from "../../app/stores/rootStore";

const NavBar: React.FC = () => {
  const rootStore = useContext(RootStoreContext);
  const { user, logout } = rootStore.userStore;
  return (
    <div>
      <Menu fixed="top" inverted>
        <Container>
          <Menu.Item header as={NavLink} exact to="/">
            {/* inline styling need to be in an object */}
            <img
              src="/assets/wave.png"
              alt="logo"
              style={{ marginRight: 10 }}
            />
            Nazare
          </Menu.Item>
          <Menu.Item name="Pogoda" as={NavLink} to="/weather"/>
          <Menu.Item name="Wydarzenia" as={NavLink} to="/activities" />
          <Menu.Item>
            <Button
              as={NavLink}
              to={"/createActivity"}
              positive
              content="Stwórz wydarzenie"
            />
          </Menu.Item>
          {user && (
            <Menu.Item position="right">
              <Image avatar spaced="right" src={user.image || "/assets/user.png"} />
              <Dropdown pointing="top left" text={user.displayName}>
                <Dropdown.Menu>
                  <Dropdown.Item
                    as={Link}
                    to={`/profile/${user.username}`}
                    text="Mój profil"
                    icon="user"
                  />
                  <Dropdown.Item onClick={logout} text="Wyloguj" icon="power" />
                </Dropdown.Menu>
              </Dropdown>
            </Menu.Item>
          )}
        </Container>
      </Menu>
    </div>
  );
};

export default observer(NavBar);
