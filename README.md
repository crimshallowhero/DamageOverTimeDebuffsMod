# DamageOverTimeDebuffsMod
Increases damage of damaging debuffs, according to max health of the target.
Buffed debuffs: On Fire, Frostburn, Cursed Inferno, Poisoned, Venom, Shadow Flame, Life Drain
Most debuffs from Calamity, Spirit and Tremor are supported.

Formula of damage is

(max_life / 70) ^ (0.4 + 0.3 * 1000 / (max_life + 1000)) * 10 * (0.1 * base_damage) ^ 0.05 * log12(base_damage + 1)
