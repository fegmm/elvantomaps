# elvantomaps
A map enabling members of the church to see the location of all other members

Environments Variables to be set:
- CLIENT_ID*
- CLIENT_SECRET*
- PERSON_CATEGORY
- GOOGLE_KEY*
- PERSON_DISPLAY_FIELDS_NAMES (",Vorname,Nachname,Alter")
- PERSON_DISPLAY_FIELDS_Value ("picture,firstname,lastname,age")
- ELVANTO_ADDITIONAL_FIELDS ("birthday")

* = Required

Files that should be mounted or in volumes
- /app/Locations.db
