type Updater<T> = (value: T) => void;

interface LogMessage {
  id: string;
  message: string;
  important: boolean;
}
