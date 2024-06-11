import "./temp.css";
import "./eventPage.css";
import "./css/EventDetails.css"
import { BrowserRouter, Routes, Route } from "react-router-dom";
import { MainNav } from "./components/shared/MainNav";
import { HomePage } from "./pages/HomePage";
import { ProfilePage } from "./pages/ProfilePage";
import { TicketsPage } from "./pages/TicketsPage";
import { EventsPage } from "./pages/EventsPage";
import { CreateEventPage } from "./pages/CreateEventPage";
import { OrgProfilePage } from "./pages/OrgProfilePage";
import { EventDetails } from "./pages/EventDetails";

function App() {
  return (
    <BrowserRouter>
      <MainNav version={"user"} />
      <main>
        <Routes>
          <Route path="/" element={<HomePage />} />
          <Route path="profile" element={<ProfilePage />} />
          <Route path="tickets" element={<TicketsPage />} />
          <Route path="events" element={<EventsPage />} />
          <Route path="createEvent" element={<CreateEventPage />} />
          <Route path="events/details/:id" element={<EventDetails />} />
          <Route path="orgProfile" element={<OrgProfilePage />} />
        </Routes>

        {/* <ProfileForm userEmail={"Eksempel@epost.no"} /> */}
        {/* <button onClick={() => testClick()}> Test </button> */}
      </main>
    </BrowserRouter>
  );
}

export default App;
