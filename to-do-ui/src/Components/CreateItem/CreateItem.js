import { Dialog, DialogTitle } from "@mui/material";

function CreateItemDialog(props) {
  const { onClose, open } = props;
  const handleClose = () => {
    onClose();
  };

  const handleListItemClick = (value) => {
    onClose(value);
  };

  return (
    <Dialog onClose={handleClose} open={open}>
      <DialogTitle> Create Item </DialogTitle>
    </Dialog>
  );
}

export default CreateItemDialog;
