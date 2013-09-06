cd /home/marcis/TILDE/RESOURCES/TRANSLIT_TRAINING_DATA/
#Encapsulate training data with <s> tags.
/home/marcis/LU/irstlm/bin/add-start-end.sh  < /home/marcis/TILDE/RESOURCES/TRANSLIT_TRAINING_DATA/mono.lv > /home/marcis/TILDE/RESOURCES/TRANSLIT_TRAINING_DATA/mono.sb.lv
/home/marcis/LU/irstlm/bin/add-start-end.sh  < /home/marcis/TILDE/RESOURCES/TRANSLIT_TRAINING_DATA/mono.lt > /home/marcis/TILDE/RESOURCES/TRANSLIT_TRAINING_DATA/mono.sb.lt
/home/marcis/LU/irstlm/bin/add-start-end.sh  < /home/marcis/TILDE/RESOURCES/TRANSLIT_TRAINING_DATA/mono.et > /home/marcis/TILDE/RESOURCES/TRANSLIT_TRAINING_DATA/mono.sb.et
/home/marcis/LU/irstlm/bin/add-start-end.sh  < /home/marcis/TILDE/RESOURCES/TRANSLIT_TRAINING_DATA/mono.hr > /home/marcis/TILDE/RESOURCES/TRANSLIT_TRAINING_DATA/mono.sb.hr
/home/marcis/LU/irstlm/bin/add-start-end.sh  < /home/marcis/TILDE/RESOURCES/TRANSLIT_TRAINING_DATA/mono.de > /home/marcis/TILDE/RESOURCES/TRANSLIT_TRAINING_DATA/mono.sb.de
/home/marcis/LU/irstlm/bin/add-start-end.sh  < /home/marcis/TILDE/RESOURCES/TRANSLIT_TRAINING_DATA/mono.ro > /home/marcis/TILDE/RESOURCES/TRANSLIT_TRAINING_DATA/mono.sb.ro
/home/marcis/LU/irstlm/bin/add-start-end.sh  < /home/marcis/TILDE/RESOURCES/TRANSLIT_TRAINING_DATA/mono.sl > /home/marcis/TILDE/RESOURCES/TRANSLIT_TRAINING_DATA/mono.sb.sl
/home/marcis/LU/irstlm/bin/add-start-end.sh  < /home/marcis/TILDE/RESOURCES/TRANSLIT_TRAINING_DATA/mono.el > /home/marcis/TILDE/RESOURCES/TRANSLIT_TRAINING_DATA/mono.sb.el
#Build a language model with IRSTLM
export IRSTLM=/home/marcis/LU/irstlm; /home/marcis/LU/irstlm/bin/build-lm.sh -n 5 -i mono.sb.lv -t ./tmp -p -s improved-kneser-ney -o mono.lm.lv
/home/marcis/LU/irstlm/bin/compile-lm --text yes mono.lm.lv.gz mono.arpa.lv
export IRSTLM=/home/marcis/LU/irstlm; /home/marcis/LU/irstlm/bin/build-lm.sh -n 5 -i mono.sb.lt -t ./tmp -p -s improved-kneser-ney -o mono.lm.lt
/home/marcis/LU/irstlm/bin/compile-lm --text yes mono.lm.lt.gz mono.arpa.lt
export IRSTLM=/home/marcis/LU/irstlm; /home/marcis/LU/irstlm/bin/build-lm.sh -n 5 -i mono.sb.et -t ./tmp -p -s improved-kneser-ney -o mono.lm.et
/home/marcis/LU/irstlm/bin/compile-lm --text yes mono.lm.et.gz mono.arpa.et
export IRSTLM=/home/marcis/LU/irstlm; /home/marcis/LU/irstlm/bin/build-lm.sh -n 5 -i mono.sb.hr -t ./tmp -p -s improved-kneser-ney -o mono.lm.hr
/home/marcis/LU/irstlm/bin/compile-lm --text yes mono.lm.hr.gz mono.arpa.hr
export IRSTLM=/home/marcis/LU/irstlm; /home/marcis/LU/irstlm/bin/build-lm.sh -n 5 -i mono.sb.de -t ./tmp -p -s improved-kneser-ney -o mono.lm.de
/home/marcis/LU/irstlm/bin/compile-lm --text yes mono.lm.de.gz mono.arpa.de
export IRSTLM=/home/marcis/LU/irstlm; /home/marcis/LU/irstlm/bin/build-lm.sh -n 5 -i mono.sb.ro -t ./tmp -p -s improved-kneser-ney -o mono.lm.ro
/home/marcis/LU/irstlm/bin/compile-lm --text yes mono.lm.ro.gz mono.arpa.ro
export IRSTLM=/home/marcis/LU/irstlm; /home/marcis/LU/irstlm/bin/build-lm.sh -n 5 -i mono.sb.sl -t ./tmp -p -s improved-kneser-ney -o mono.lm.sl
/home/marcis/LU/irstlm/bin/compile-lm --text yes mono.lm.sl.gz mono.arpa.sl
export IRSTLM=/home/marcis/LU/irstlm; /home/marcis/LU/irstlm/bin/build-lm.sh -n 5 -i mono.sb.el -t ./tmp -p -s improved-kneser-ney -o mono.lm.el
/home/marcis/LU/irstlm/bin/compile-lm --text yes mono.lm.el.gz mono.arpa.el
#Binarize the language model
/home/marcis/LU/moses/mosesdecoder/bin/build_binary -i mono.arpa.lv  mono.blm.lv
/home/marcis/LU/moses/mosesdecoder/bin/build_binary -i mono.arpa.lt  mono.blm.lt
/home/marcis/LU/moses/mosesdecoder/bin/build_binary -i mono.arpa.et  mono.blm.et
/home/marcis/LU/moses/mosesdecoder/bin/build_binary -i mono.arpa.hr  mono.blm.hr
/home/marcis/LU/moses/mosesdecoder/bin/build_binary -i mono.arpa.de  mono.blm.de
/home/marcis/LU/moses/mosesdecoder/bin/build_binary -i mono.arpa.ro  mono.blm.ro
/home/marcis/LU/moses/mosesdecoder/bin/build_binary -i mono.arpa.sl  mono.blm.sl
/home/marcis/LU/moses/mosesdecoder/bin/build_binary -i mono.arpa.el  mono.blm.el
#Checking if it works:
echo "s k o l a" | /home/marcis/LU/moses/mosesdecoder/bin/query mono.blm.lv
echo "m o k y k l a" | /home/marcis/LU/moses/mosesdecoder/bin/query mono.blm.lt
echo "k o o l" | /home/marcis/LU/moses/mosesdecoder/bin/query mono.blm.et
echo "š k o l a" | /home/marcis/LU/moses/mosesdecoder/bin/query mono.blm.hr
echo "s c h u l e" | /home/marcis/LU/moses/mosesdecoder/bin/query mono.blm.de
echo "ș c o a l ă" | /home/marcis/LU/moses/mosesdecoder/bin/query mono.blm.ro
echo "š o l a" | /home/marcis/LU/moses/mosesdecoder/bin/query mono.blm.sl
echo "σ χ ο λ ε ί ο" | /home/marcis/LU/moses/mosesdecoder/bin/query mono.blm.el
