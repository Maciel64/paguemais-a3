export type DialogUIStates = "closed" | "deleting" | "creating" | "updating";

export interface UseUIState {
  dialogUIState: DialogUIStates;
  setDialogUIState: (UIState: DialogUIStates) => void;
}
