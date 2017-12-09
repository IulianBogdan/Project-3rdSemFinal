INSERT INTO ItemType(Name) VALUES ('Phone'),('Television')
GO
INSERT INTO PropertyType(Name) VALUES ('bool'),('int'),('long'),('double'),('string'),('resolution')
GO
INSERT INTO Property(Name, PropertyTypeId) VALUES ('Screen resolution', 6), ('4g', 1), ('HDMI ports', 2)
GO
INSERT INTO ItemTypeDefaultProperty VALUES (1, 1, '720x1080'), (1, 2, 'false'), (2, 1, '1920x1080'), (2, 3, '1')