-- Run with psql after migrations and after registering freshout-demo in the app.
BEGIN;
DO $$
DECLARE
    demo_user integer;
    fridge integer;
    pantry integer;
    shopping integer;
BEGIN
    SELECT "UserId" INTO STRICT demo_user
    FROM "Users" WHERE "Username" = 'freshout-demo';

    IF EXISTS (SELECT 1 FROM "LocationLists" WHERE "UserId" = demo_user)
        OR EXISTS (SELECT 1 FROM "GroceryLists" WHERE "UserId" = demo_user) THEN
        RAISE EXCEPTION 'Use a fresh freshout-demo account with empty lists.';
    END IF;

    INSERT INTO "LocationLists" ("Name", "UserId") VALUES ('Fridge', demo_user)
        RETURNING "LocationListId" INTO fridge;
    INSERT INTO "LocationLists" ("Name", "UserId") VALUES ('Pantry', demo_user)
        RETURNING "LocationListId" INTO pantry;
    INSERT INTO "GroceryLists" ("Name", "UserId") VALUES ('Weekly groceries', demo_user)
        RETURNING "GroceryListId" INTO shopping;

    INSERT INTO "FoodItems"
        ("Discriminator", "Name", "Type", "Quantity", "UserId", "LocationListId", "ExpirationDate")
    VALUES
        ('LocationItem', 'Greek yogurt', '🥛 Dairy', 2, demo_user, fridge, CURRENT_DATE - 1),
        ('LocationItem', 'Carrots', '🥕 Vegetables', 4, demo_user, fridge, CURRENT_DATE + 2),
        ('LocationItem', 'Oats', '🌾 Grains', 1, demo_user, pantry, CURRENT_DATE + 45);

    INSERT INTO "FoodItems"
        ("Discriminator", "Name", "Type", "Quantity", "UserId", "GroceryListId", "IsChecked")
    VALUES
        ('GroceryItem', 'Milk', '🥛 Dairy', 1, demo_user, shopping, true),
        ('GroceryItem', 'Bell peppers', '🥕 Vegetables', 3, demo_user, shopping, false);
END;
$$;
COMMIT;
