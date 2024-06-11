import "../css/EventDetails.css";
import { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import { getEventById } from "../services/tempService";
// import { events } from "./EventsPage";
import dayjs from "dayjs";
import CustomParseFormat from "dayjs/plugin/customParseFormat";

export const EventDetails = () => {
  const { id } = useParams();
  const [event, setEvent] = useState([{}]);
  const [loading, setLoading] = useState(true);
  const [dateTime, setDateTime] = useState({
    timeStart: "",
    timeEnd: "",
    dateStart: "",
    dateEnd: "",
  });

  useEffect(() => {
    loadEvent();
  }, []);

  const loadEvent = async () => {
    const result = await getEventById(id);
    const testDate = dayjs(event.startTime).format("DD-MM-YYYY");

    console.log(dateTime);
    setEvent(result);
    if (result != undefined && result != null) {
      setLoading(false);
      splitDateTime(result);
    }
  };

  const splitDateTime = (eventevent) => {
    dayjs.extend(CustomParseFormat);

    setDateTime({
      timeStart: dayjs(eventevent.startTime).format("HH:mm"),
      timeEnd: dayjs(eventevent.endTime).format("HH:mm"),
      dateStart: dayjs(eventevent.startTime).format("DD.MM.YYYY"),
      dateEnd: dayjs(eventevent.endTime).format("DD.MM.YYYY"),
    });
    console.log(dateTime);
  };

  if (loading) {
    return <p>loading....</p>;
  }
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
            <p>{event.capacity}</p>
          </div>
          <div className="infoItem">
            <p>Aldersgrense:</p>
            <p>{event.ageLimit}</p>
          </div>
          <div className="infoItem">
            <p>privat?: </p>
            <p>{event.private ? "ja" : "nei"}</p>
          </div>
          <div className="infoItem">
            <p>Organisasjon:</p>
            <p> {event?.organization?.name}</p>
          </div>
          <div className="infoItem">
            <p>Kontaktperson:</p>
            <p> {event?.contactPerson?.name}</p>
          </div>
        </div>
      </div>
      <div className="eventDetailsRight">
        <div className="eventInfo">
          <h1>{event.title}</h1>
          <div className="dateTimePlace">
            {dateTime.dateStart === dateTime.dateEnd ? (
              <>
                <div className="dateTime">
                  <div className="dateItem">
                    <i className="bi bi-calendar3 icon"></i>
                    <p>{dateTime.dateStart}</p>
                  </div>
                  <div className="timeItem">
                    <i className="bi bi-clock icon"></i>
                    <p>{`${dateTime.timeStart} - ${dateTime.timeEnd}`}</p>
                  </div>
                </div>
              </>
            ) : (
              <>
                <div className="dateTime">
                  <div>
                    <div className="dateItem">
                      <i className="bi bi-calendar3 icon"></i>
                      <p>{dateTime.dateStart}</p>
                    </div>
                    <div className="dateItem">
                      <i className="bi bi-calendar3 icon"></i>
                      <p>{dateTime.dateEnd}</p>
                    </div>
                  </div>
                  <div>
                    <div className="timeItem">
                      <i className="bi bi-clock icon"></i>
                      <p>{`${dateTime.timeStart}`}</p>
                    </div>
                    <div className="timeItem">
                      <i className="bi bi-clock icon"></i>
                      <p>{`${dateTime.timeEnd} `}</p>
                    </div>
                  </div>
                </div>
              </>
            )}
            <div className="place">
              <i className="bi bi-geo-alt icon"></i>
              <p>{event.place.location}</p>
            </div>
          </div>
        </div>
        <div className="eventText">
          <p>{event.description}</p>
        </div>
        <div className="btnDiv">
          <button className="buttons">Rediger</button>
          <button className="publishBtn buttons">Publiser</button>
        </div>
      </div>
    </div>
  );
};

{
  /* <h1>{event.title}</h1>
<p>DATO</p>
<p>Beskrivelse: {event.description}</p>
<p>Antall plasser: {event.capacity}</p>
<p>Aldersgrense: {event.ageLimit}</p>
<p>privat?: {event.private ? "ja" : "nei"}</p>
<p>publisert?: {event.published ? "ja" : "nei"}</p>
<p>organisasjon: {event?.organization?.name}</p>
<p>lokasjon: {event?.place?.location}</p>
<p>kontaktperson: {event?.contactPerson?.name}</p>
<p>start: {event.startTime}</p>
<p>slutt: {event.endTime}</p> */
}
