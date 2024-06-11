//import { useState } from "react";
import { EventItem } from "../components/EventItem";
//import { getEvents } from "../services/tempService";
export const events = [
  {
    id: 1,
    title: "Pils & Prog",
    date: "14. Mai 2024",
    sold: 40,
    waitlist: 11,
    available: 0
  },
  {
    id: 2,
    title: "Bedpress",
    date: "14. April 2024",
    sold: 30,
    waitlist: 0,
    available: 4
  },
  {
    id: 3,
    title: "Sommeravslutning",
    date: "12. Juni 2024",
    sold: 240,
    waitlist: 0,
    available: 50
  },
  {
    id: 4,
    title: "Kickoff",
    date: "11. Januar 2024",
    sold: 190,
    waitlist: 22,
    available: 0
  },
];
export const EventsPage = () => {
  // = getEvents()

  return (
    <>
      <header className="pageHeader">
        <h1 className="pageTitle">Kommende arrangementer</h1>
      </header>
        <main className="eventsPageContainer">
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
