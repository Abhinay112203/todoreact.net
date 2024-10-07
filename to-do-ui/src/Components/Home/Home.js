import { Button } from "@mui/material";
import logo from "../../logo.svg";
import "./Home.css";
import CreateItemDialog from "../CreateItem/CreateItem";
import { useState } from "react";
function Home() {
  const [open, setOpen] = useState(false);
  const handleClickOpen = () => {
    setOpen(true);
  };

  const handleClose = (value) => {
    setOpen(false);
  };
  return (
    <div>
      <header className="">
        <Button variant="contained" onClick={handleClickOpen}>
          Add Item
        </Button>
        <CreateItemDialog open={open} onClose={handleClose} />
      </header>
    </div>
  );
}
export default Home;
