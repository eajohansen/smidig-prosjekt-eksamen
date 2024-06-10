import { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import { getEventById } from "../services/tempService";
// import { events } from "./EventsPage";

export const EventDetails = () => {
  const { id } = useParams();
  const [event, setEvent] = useState([{}]);
  const [loading, setLoading] = useState(true);
  useEffect(() => {
    loadEvent();
  }, []);

  const loadEvent = async () => {
    const result = await getEventById(id);

    setEvent(result);
    if (result != undefined && result != null) {
      setLoading(false);
    }
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
      </div>
      <div className="eventDetailsRight">
        <h1>{event.title}</h1>
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
        <p>slutt: {event.endTime}</p>
        <div className="btnDiv">
          <button>Rediger</button>
          <button>Publiser</button>
        </div>
      </div>
    </div>
  );
};
