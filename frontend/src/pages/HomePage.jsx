import React from "react";
//import {EventItem} from "../components/EventItem.jsx";
//import {events} from "./EventsPage.jsx";
export const follower = [
  {
    followersTotal: 122,
    followersLastMonth: 39,
    growth: 83,
  },
];
export const HomePage = () => {
  // return <LoginPopup />; ??
  return (
    <>
      <header className="homePageHeader">
        <h1 className="homePageTitle">HJEM</h1>
      </header>
      <main className="homePageContainer">
        <div className="followerContainer">
          <FollowersBox
            followers={follower.followersTotal}
            followersLM={follower.followersLastMonth}
            growth={follower.growth}
          />
        </div>
        <div className="eventContainer"></div>
        <div className="createEventContainer"></div>
        <div className="calendar"></div>
      </main>
    </>
  );
};
