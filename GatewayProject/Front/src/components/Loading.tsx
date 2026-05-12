import React from 'react';

interface LoadingProps {
  message?: string;
}

export function Loading({ message = 'Carregando...' }: LoadingProps) {
  return (
    <div className="flex items-center justify-center py-12">
      <div className="text-center">
        <div className="inline-flex items-center justify-center">
          <div className="w-8 h-8 border-4 border-blue-200 border-t-blue-600 rounded-full animate-spin"></div>
        </div>
        <p className="mt-4 text-gray-600">{message}</p>
      </div>
    </div>
  );
}
