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
    console.log(result);
    setEvents(result);
  };
  return (
    <>
      <div className="pageHeader">
        <h1 className="pageTitle">Kommende arrangementer</h1>
      </div>
      <main className="eventsPageContainer">
        {events != null &&
          events.map((event, key) => (
            <EventItem
              key={key}
              id={event?.eventId}
              address={event?.place?.location}
              start={event.startTime}
              end={event.endTime}
              title={event?.title}
              capacity={event?.capacity}
            />
          ))}
      </main>
    </>
  );
};
