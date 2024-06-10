import React from "react";
import { EventItem } from "../components/EventItem.jsx";
//import {events} from "./EventsPage.jsx";
import { MainNav } from "../components/shared/MainNav";
import FullCalendar from "@fullcalendar/react";
import dayGridPlugin from "@fullcalendar/daygrid";
// const follower= [
//   {
//     followersTotal: 122,
//     followersLastMonth: 39,
//     growth: 83
//   }
// ];
export const HomePage = () => {
  //  return <LoginPopup />;
  return (
    <>
      <MainNav />
      <header className="homePageHeader">
        <h1 className="homePageTitle">Hjem</h1>
      </header>

      <main className="homePageContainer">
        <div className="eventContainer">
          <label htmlFor="">Førstkommende Arrangement</label>
          <div className="eventObject">
            <EventItem />
          </div>
        </div>

        <div className="followerContainer">
          <label htmlFor="">Vekst</label>
          <div>
            <label htmlFor="">Følgere</label>
            {/* <FollowersBox
                      followers={follower.followersTotal}
                      followersLM={follower.followersLastMonth}
                      growth={follower.growth}
                      /> */}
            <button>Vis liste over følgere</button>
          </div>
        </div>
        <div className="calendar">
          <label htmlFor="">Oversikt</label>
          <FullCalendar plugins={[dayGridPlugin]} initialView="dayGridMonth" />
        </div>

        <div className="createEventContainer">
          <label htmlFor="">Legg til nytt arrangement</label>
          <button></button>
        </div>
      </main>
    </>
  );
};
