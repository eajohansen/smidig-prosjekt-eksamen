import "../css/homepage.css";
import { EventItem } from "../components/EventItem.jsx";
import FullCalendar from "@fullcalendar/react";
import dayGridPlugin from "@fullcalendar/daygrid";
import { Link } from "react-router-dom";
import { isAdmin } from "../services/tempService.js";
import { useState, useEffect } from "react";

export const HomePage = () => {
  const [isAdminOrOrg, setIsAdminOrOrg] = useState(false);

  useEffect(() => {
    isUserAdmin();
  }, []);

  const isUserAdmin = async () => {
    const adminResult = await isAdmin();
    if (adminResult.data.admin || adminResult.data.organizer) {
      setIsAdminOrOrg(true);
    }
  };

  return (
    <>
      {localStorage.getItem("organizator") === "true" || localStorage.getItem("admin") === "true" ? (<main className="homePageContainer">
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

        <div className="calender">
          <label htmlFor="">Oversikt</label>
          <FullCalendar plugins={[dayGridPlugin]} initialView="dayGridMonth" />
        </div>
        <div className="createEventContainer">
          <label className="createEventTxt" htmlFor="">
            Legg til nytt arrangement
          </label>
          {isAdminOrOrg ? (
            <>
              <Link to={"createEvent"}>
                <button className="createEventButton">
                  <i className="bi bi-plus-circle circleIcon"></i>
                </button>
              </Link>
            </>
          ) : (
            <>
              <button
                className="createEventButton"
                onClick={() => {
                  alert("you are not admin or organizer!");
                }}
              >
                <i className="bi bi-plus-circle circleIcon"></i>
              </button>
            </>
          )}
        </div>
      </main>) : (<main><h1>User dashboard</h1> </main>) }
    </>
  );
};
