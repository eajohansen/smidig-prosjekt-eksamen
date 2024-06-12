import "../css/homepage.css";
import { EventItem } from "../components/EventItem.jsx";
import FullCalendar from "@fullcalendar/react";
import dayGridPlugin from "@fullcalendar/daygrid";
import { Link } from "react-router-dom";

export const HomePage = () => {
  return (
    <>
      <main className="homePageContainer">
        <div className="eventHomeContainer">
          <label htmlFor="">Førstkommende Arrangement</label>
          <div className="eventObject">
            <EventItem />
          </div>
        </div>
        <div className="followerContainer">
          <label htmlFor="">Vekst</label>
          <div className="vekstobject">
            <h4>Følgere</h4>
            <div className="infoFollowers">
              <div>
                <p>Antall følgere</p>
                <p>5</p>
                <p>følgere forrige måned</p>
                <p>5</p>
              </div>
              <div>
                <p>vekst</p>
                <p>9</p>
              </div>
            </div>
            <button className="seeEventButton showFollowers">
              Vis liste over følgere
            </button>
          </div>
        </div>
        <div className="calendar">
          <label htmlFor="">Oversikt</label>
          <FullCalendar plugins={[dayGridPlugin]} initialView="dayGridMonth" />
        </div>
        <div className="createEventContainer">
          <label className="createEventTxt" htmlFor="">
            Legg til nytt arrangement
          </label>
          <Link to={"createEvent"}>
            <button className="createEventButton">
              <i className="bi bi-plus-circle circleIcon"></i>
            </button>
          </Link>
        </div>
      </main>
    </>
  );
};
