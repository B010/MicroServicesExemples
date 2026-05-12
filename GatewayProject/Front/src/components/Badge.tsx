import React from 'react';

interface BadgeProps {
  variant?: 'success' | 'warning' | 'danger' | 'info';
  children: React.ReactNode;
  className?: string;
}

export function Badge({ variant = 'info', children, className }: BadgeProps) {
  const variantStyles = {
    success: 'bg-green-100 text-green-800',
    warning: 'bg-yellow-100 text-yellow-800',
    danger: 'bg-red-100 text-red-800',
    info: 'bg-blue-100 text-blue-800',
  };

  return (
    <span className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-sm font-medium ${variantStyles[variant]} ${className || ''}`}>
      {children}
    </span>
  );
}
