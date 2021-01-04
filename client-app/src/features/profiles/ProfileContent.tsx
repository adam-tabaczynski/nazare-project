import React from "react";
import { Tab } from "semantic-ui-react";
import ProfileActivities from "./ProfileActivities";
import ProfileDescription from "./ProfileDescription";
import ProfileFollowings from "./ProfileFollowings";
import ProfilePhotos from "./ProfilePhotos";

const panes = [
  { menuItem: "Bio", render: () => <ProfileDescription /> },
  { menuItem: "Zdjęcia", render: () => <ProfilePhotos /> },
  {
    menuItem: "Aktywności",
    render: () => <ProfileActivities />,
  },
  {
    menuItem: "Obserwujący",
    render: () => <ProfileFollowings />,
  },
  {
    menuItem: "Obserwujesz",
    render: () => <ProfileFollowings />,
  },
];

interface IProps {
  setActiveTab: (activeIndex: any) => void;
}

const ProfileContent: React.FC<IProps> = ({ setActiveTab }) => {
  return (
    <Tab
      menu={{ fluid: true, vertical: true }}
      menuPosition="right"
      panes={panes}
      onTabChange={(e, data) => setActiveTab(data.activeIndex)}
    />
  );
};

export default ProfileContent;
