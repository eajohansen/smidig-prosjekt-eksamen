import { useState } from "react";
import { EventItem } from "../components/EventItem";
import { getEvents } from "../services/tempService";
export const EventsPage = () => {
  const [events, setEvents] = useState([
    {
      id: 1,
      title: "bursdag",
      date: "14.mai.2002",
    },
    {
      id: 2,
      title: "bursdag",
      date: "14.mai.2002",
    },
    {
      id: 3,
      title: "bursdag",
      date: "14.mai.2002",
    },
  ]); // = getEvents()

  return (
    <>
      <main className="eventsPageContainer">
        {events.map((event) => (
          <EventItem
            key={event.id}
            id={event.id}
            title={event.title}
            date={event.date}
          />
        ))}
      </main>
    </>
  );
};
