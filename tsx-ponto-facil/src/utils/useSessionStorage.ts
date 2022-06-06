import React, { useState, useEffect } from 'react';

export function getSessionStorageOrDefault<T>(key: string, defaultValue: T): T {
  const stored = sessionStorage.getItem(key);
  if (!stored) {
    return defaultValue;
  }
  return JSON.parse(stored);
}

export function setSessionStorage<T>(key: string, value: T): void {
  sessionStorage.setItem(key, JSON.stringify(value));
}

export function useSessionStorage<T>(key: string, defaultValue: T): [T, React.Dispatch<React.SetStateAction<T>>] {
  const [value, setValue] = useState(
    getSessionStorageOrDefault(key, defaultValue)
  );

  useEffect(() => {
    sessionStorage.setItem(key, JSON.stringify(value));
  }, [key, value]);

  return [value, setValue];
}