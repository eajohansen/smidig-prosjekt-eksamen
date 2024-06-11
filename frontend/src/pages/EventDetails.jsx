import React from "react";
import { useParams } from "react-router-dom";
import { events } from "./EventsPage";

export const EventDetails = () => {
  const { id } = useParams();
  const lorem =
    "Lorem ipsum dolor sit amet consectetur, adipisicing elit. Tenetur, nobis ipsa! Ad nemo cum atque assumenda accusantium magnam cumque pariatur nostrum delectus doloribus labore blanditiis saepe, incidunt doloremque temporibus possimus!" +
      "Lorem ipsum dolor sit amet consectetur, adipisicing elit. Tenetur, nobis ipsa! Ad nemo cum atque assumenda accusantium magnam cumque pariatur nostrum delectus doloribus labore blanditiis saepe, incidunt doloremque temporibus possimus!" +
      "Lorem ipsum dolor sit amet consectetur, adipisicing elit. Tenetur, nobis ipsa! Ad nemo cum atque assumenda accusantium magnam cumque pariatur nostrum delectus doloribus labore blanditiis saepe, incidunt doloremque temporibus possimus!" +
      "Lorem ipsum dolor sit amet consectetur, adipisicing elit. Tenetur, nobis ipsa! Ad nemo cum atque assumenda accusantium magnam cumque pariatur nostrum delectus doloribus labore blanditiis saepe, incidunt doloremque temporibus possimus!" +
      "Lorem ipsum dolor sit amet consectetur, adipisicing elit. Tenetur, nobis ipsa! Ad nemo cum atque assumenda accusantium magnam cumque pariatur nostrum delectus doloribus labore blanditiis saepe, incidunt doloremque temporibus possimus!" +
      "Lorem ipsum dolor sit amet consectetur, adipisicing elit. Tenetur, nobis ipsa! Ad nemo cum atque assumenda accusantium magnam cumque pariatur nostrum delectus doloribus labore blanditiis saepe, incidunt doloremque temporibus possimus!";
  const event = events.find((event) => event.id === parseInt(id));
  return (
    <div className="eventDetailsContainer">
      <div className="eventDetailsLeft">
        <img
          src="https://images.unsplash.com/photo-1717684566059-4d16b456c72a?q=80&w=2938&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D"
          alt=""
        />
          <div className="infoContainer">
              <div className="infoItem">
                  <p>Ledige Plasser:</p>
                  <p>16</p>
              </div>
              <div className="infoItem">
                  <p>Aldersgrense:</p>
                  <p>18</p>
              </div>
              <div className="infoItem">
                  <p>Matservering:</p>
                  <p>JA</p>
              </div>
              <div className="infoItem">
                  <p>Gratis arrangement:</p>
                  <p>Ja</p>
              </div>
          </div>
      </div>
        <div className="eventDetailsRight">
            <div className="eventInfo">
                <h1>{event.title}</h1>
                <div className="dateTimePlace">
                    <div className="dateTime">
                        <div className="dateItem">
                        <i className="bi bi-calendar3 icon"></i>
                            <p>{event.date}</p>
                        </div>
                        <div className="timeItem">
                            <i className="bi bi-clock icon"></i>
                                <p>17:00 - 21:00</p>
                        </div>
                    </div>
                    <div className="place">
                        <i className="bi bi-geo-alt icon"></i>
                        <p>Joachim Nielsens gang 17, 0000 Oslo</p>
                    </div>
                </div>
            </div>
            <div className="eventText">
            <p>{lorem}</p>
          </div>
        <div className="btnDiv">
          <button className="buttons">Rediger</button>
          <button className="publishBtn buttons">Publiser</button>
        </div>
      </div>
    </div>
  );
};
