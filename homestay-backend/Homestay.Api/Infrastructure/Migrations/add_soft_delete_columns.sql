-- Migration script để thêm soft delete columns vào database

-- =============================================
-- Add Soft Delete Columns to Users Table
-- =============================================
ALTER TABLE core.users 
ADD COLUMN deleted_at TIMESTAMP WITHOUT TIME ZONE NULL,
ADD COLUMN deleted_by UUID NULL;

-- Create index for better query performance
CREATE INDEX ix_users_deleted_at ON core.users(deleted_at);

-- =============================================
-- Add Soft Delete Columns to Homestays Table
-- =============================================
ALTER TABLE core.homestays 
ADD COLUMN deleted_at TIMESTAMP WITHOUT TIME ZONE NULL,
ADD COLUMN deleted_by UUID NULL;

CREATE INDEX ix_homestays_deleted_at ON core.homestays(deleted_at);

-- =============================================
-- Add Soft Delete Columns to Bookings Table
-- =============================================
ALTER TABLE booking.bookings 
ADD COLUMN deleted_at TIMESTAMP WITHOUT TIME ZONE NULL,
ADD COLUMN deleted_by UUID NULL;

CREATE INDEX ix_bookings_deleted_at ON booking.bookings(deleted_at);

-- =============================================
-- Add Soft Delete Columns to Reviews Table (Optional)
-- =============================================
ALTER TABLE review.reviews 
ADD COLUMN deleted_at TIMESTAMP WITHOUT TIME ZONE NULL,
ADD COLUMN deleted_by UUID NULL;

CREATE INDEX ix_reviews_deleted_at ON review.reviews(deleted_at);
-- =============================================
-- Add Soft Delete Columns to Rooms Table (Optional)
-- =============================================
ALTER TABLE core.rooms 
ADD COLUMN deleted_at TIMESTAMP WITHOUT TIME ZONE NULL,
ADD COLUMN deleted_by UUID NULL;

CREATE INDEX ix_rooms_deleted_at ON core.rooms(deleted_at);

-- =============================================
-- Add Soft Delete Columns to Promotions Table (Optional)
-- =============================================
ALTER TABLE marketing.promotions 
ADD COLUMN deleted_at TIMESTAMP WITHOUT TIME ZONE NULL,
ADD COLUMN deleted_by UUID NULL;

CREATE INDEX ix_promotions_deleted_at ON marketing.promotions(deleted_at);

