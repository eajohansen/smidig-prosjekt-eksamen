import React from "react";
import { Link } from "react-router-dom";

export const EventItem = (props) => {
  const { title, date, id, sold, waitlist, available } = props;
  return (
    <article key={id} className="eventItemContainer">
      <div className="eventImageContainer">
        <img
          className="eventImage"
          src="https://images.unsplash.com/photo-1717684566059-4d16b456c72a?q=80&w=2938&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D"
          alt="random"
        />
        <div className="eventDetails">
        <h3>{title}</h3>
        <p>{date}</p>
        </div>
        </div>
        <div className="ticketInfoContainer">
          <h3>Billettstatus</h3>
          <div className="ticketInfo">

          <div className="">
            <p>Solgt</p>
            <h2>{sold}</h2>
            <p>Ledig</p>
            <h2 className="available">{available}</h2>
          </div>
          <div className="WaitinglistContainer">
            <p>Venteliste</p>
            <h2 className="waitlist">{waitlist}</h2>
          </div>
        </div>
        <div className="buttonContainer">
          <button className="viewRegistrationsButton">Vis påmeldte</button>
          <Link to={`details/${id}`}>
            <button className="seeEventButton">Se arrangement</button>
          </Link>
        </div>
      </div>
    </article>
  );
};
