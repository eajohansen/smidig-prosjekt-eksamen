import React, { useState } from "react";
import { sendEvent } from "../services/tempService";
import "../temp.css";
import "../css/CreateEventPage.css";
export const CreateEventPage = () => {
  const [checkedFree, setCheckedFree] = useState();
  const organization = 1;
  const [start, setStart] = useState();
  const [end, setEnd] = useState();

  const [event, setEvent] = useState({
    title: "",
    address: "",
    contactP: "",
    description: "",
    capacity: 0,
    ageLimit: 0,
    food: false,
    free: false,
    published: false,
    start: "",
    startTime: "",
    end: "",
    endTime: "",
    org: organization,
    customField: []

  });

  const handleChange = (e) => {
    setEvent({ ...event, [e.currentTarget.name]: e.currentTarget.value });
  };

  const handleSubmit = async () => {
    const result = await sendEvent(event);
    console.log(result);
  };

  return (
      <div className="page-container">
        <h2>Opprett arrangement</h2>
        <hr></hr>
        <div className="create-event-box">
          <section className="left-box">
            <div>
              <label htmlFor="title">Tittel</label>
              <input id="title" name="title" onChange={handleChange}></input>
            </div>
            <div>
              <label htmlFor="address">Adresse</label>
              <input id="address" name="address" onChange={handleChange}></input>
            </div>
            <div>
              <label htmlFor="contactPerson">Kontakt Person</label>
              <input
                  id="contactPerson"
                  name="contactP"
                  onChange={handleChange}
              ></input>
            </div>
            <div>
              <label htmlFor="description">Beskrivelse</label>
              <input
                  id="description"
                  name="description"
                  onChange={handleChange}
              ></input>
            </div>
            <div className="event-check-box">
              <div>
                <label htmlFor="maxSeating">Antall Plasser</label>
                <input id="maxSeating" name="capacity" onChange={handleChange} />
              </div>
              <div>
                <label htmlFor="minAge">Aldersgrense</label>
                <input id="minAge" name="ageLimit" onChange={handleChange} />
              </div>
              <div>
                <label htmlFor="foodService">Matservering</label>
                <input
                    id="foodService"
                    className="checkBox"
                    type="checkbox"
                    name="food"
                    onChange={handleChange}
                />
              </div>
              <div>
                <label htmlFor="freeEvent"> Gratis arrangement </label>
                <input
                    id="freeEvent"
                    className="checkBox"
                    type="checkbox"
                    name="free"
                    onChange={handleChange}
                />
              </div>
            </div>
          </section>
          <section className="right-box">
            <label htmlFor="startTime">Start dato og kl.</label>
            <input
                id="startDate"
                type="date"
                name="start"
                onChange={handleChange}
            ></input>
            <input
                id="startTime"
                type="time"
                max="23:59"
                name="startTime"
                onChange={handleChange}
            ></input>
            <label htmlFor="endTime">Slutt dato og kl.</label>
            <input
                id="endDate"
                type="date"
                name="end"
                onChange={handleChange}
            ></input>
            <input
                id="endTime"
                type="time"
                name="endTime"
                onChange={handleChange}
            ></input>
            <div>
              <div>
                <label htmlFor="customField"></label>
                <input></input>
                <input type="checkbox"></input>
              </div>
            </div>
            <div>
              <label></label>
              <button>Last opp bilde</button>
              <button>Avbryt</button>
              <button onClick={handleSubmit}>Lagre og forhåndsvis</button>
            </div>
          </section>
        </div>
      </div>
  );
};
