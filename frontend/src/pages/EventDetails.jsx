import React from "react";
import { useParams } from "react-router-dom";
import { events } from "./EventsPage";

export const EventDetails = () => {
  const { id } = useParams();
  const lorem =
    "Lorem ipsum dolor sit amet consectetur, adipisicing elit. Tenetur, nobis ipsa! Ad nemo cum atque assumenda accusantium magnam cumque pariatur nostrum delectus doloribus labore blanditiis saepe, incidunt doloremque temporibus possimus!";
  const event = events.find((event) => event.id === parseInt(id));
  return (
    <div className="eventDetailsContainer">
      <div className="eventDetailsLeft">
        <img
          src="https://images.unsplash.com/photo-1717684566059-4d16b456c72a?q=80&w=2938&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D"
          alt=""
        />
      </div>
      <div className="eventDetailsRight">
        <h1>{event.title}</h1>
        <p>{event.date}</p>
        <p>{lorem}</p>
        <div className="btnDiv">
          <button>Rediger</button>
          <button>Publiser</button>
        </div>
      </div>
    </div>
  );
};
