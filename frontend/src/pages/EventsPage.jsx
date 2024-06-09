import { useState } from "react";
import { EventItem } from "../components/EventItem";
import { getEvents } from "../services/tempService";
export const events = [
  {
    id: 1,
    title: "Bursdag",
    date: "14.mai.2002",
    sold: 40,
    waitlist: 11,
    available: 0
  },
  {
    id: 2,
    title: "Bursdag",
    date: "14.mai.2002",
    sold: 40,
    waitlist: 11,
    available: 0
  },
  {
    id: 3,
    title: "Bursdag",
    date: "14.mai.2002",
    sold: 40,
    waitlist: 11,
    available: 0
  },
];
export const EventsPage = () => {
  // = getEvents()

  return (
    <>
        <main className="eventsPageContainer">
      <h2 className="pageTitle">Kommende arrangementer</h2>
        {events.map((event) => (
          <EventItem
            key={event.id}
            id={event.id}
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
