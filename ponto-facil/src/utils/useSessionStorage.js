import { useState, useEffect } from 'react';

export function getSessionStorageOrDefault(key, defaultValue) {
  const stored = sessionStorage.getItem(key);
  if (!stored) {
    return defaultValue;
  }
  return JSON.parse(stored);
}

export function setSessionStorage(key, value) {
  sessionStorage.setItem(key, JSON.stringify(value));
}

export function useSessionStorage(key, defaultValue) {
  const [value, setValue] = useState(
    getSessionStorageOrDefault(key, defaultValue)
  );

  useEffect(() => {
    sessionStorage.setItem(key, JSON.stringify(value));
  }, [key, value]);

  return [value, setValue];
}