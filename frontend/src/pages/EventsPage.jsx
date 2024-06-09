import { useState, useEffect } from "react";
import { EventItem } from "../components/EventItem";
import { getEvents } from "../services/tempService";
export const EventsPage = () => {
  const [events, setEvents] = useState([{}]);
  useEffect(() => {
    loadEvents();
  }, []);

  const loadEvents = async () => {
    const result = await getEvents();

    setEvents(result);
  };
  return (
    <>
      <div className="pageHeader">
        <h1 className="pageTitle">Kommende arrangementer</h1>
      </div>
      <main className="eventsPageContainer">
        {events.map((event) => (
          <EventItem
            key={event.eventId}
            id={event.eventId}
            title={event.title}
            date={event.date}
            sold={event.sold}
            waitlist={event.waitlist}
            available={event.available}
          />
        ))}
      </main>
    </>
  );
};
